import { Injectable } from '@angular/core';
import { StegomasterConfiguration } from './configuration/stegomaster-configuration';
import { StegomasterRequest } from './DTOs/stegomaster-request';
import { RGBA } from './configuration/rgba';
import { StegomasterInfoConfiguration } from './configuration/stegomaster-info-configuration';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class StegomasterService {
  constructor(private toastr: ToastrService) { }

  private stringToBinary(data: string): string {
    let result = '';
    for (var i = 0; i < data.length; i++) {
      result += data[i].charCodeAt(0).toString(2).padStart(StegomasterConfiguration.BITS_COUNT, '0');
    }
    return result;
  }

  private getBinaryData(data: string[]): string {
    let result = '';
    for (let i = 0; i < data.length; i++) {
      const delimeter = i !== data.length - 1 ? StegomasterConfiguration.DATA_DELIMETER : StegomasterConfiguration.DATA_ENDING
      result += `${this.stringToBinary(data[i])}${delimeter}`;
    }
    return result;
  }

  private calculateMinContainerPixelsCount(binaryData: string): number {
    const payloadPerPixel = StegomasterConfiguration.ColorsDataHiding.length * StegomasterConfiguration.LSB_COUNT;
    const pixelsRequiredWithoutShift = Math.floor(binaryData.length / payloadPerPixel);

    return pixelsRequiredWithoutShift * StegomasterConfiguration.SKIP_BITS_COUNT;
  }

  public async loadImageToRequest(data: string[]): Promise<HTMLImageElement> {
    await this.logInfo([`3. Use default container: ${StegomasterConfiguration.USE_DEFAULT_CONTAINER}.`]);

    const binaryData = this.getBinaryData(data);
    await this.logInfo([`4. Binary representation of entered data:`, '\n', binaryData, '\n\t', `bits count: ${binaryData.length}.`]);

    const pixelsRequired = this.calculateMinContainerPixelsCount(binaryData);
    await this.logInfo([`5. Minimum number of pixels required to the container: ${pixelsRequired}.`]);

    if (StegomasterConfiguration.USE_DEFAULT_CONTAINER) {
      const defaultContainerSize = StegomasterConfiguration.DEFAULT_CONTAINER.width * StegomasterConfiguration.DEFAULT_CONTAINER.height;
      if (pixelsRequired > defaultContainerSize) {
        await this.LogError('Error: default container for response data is incorrect. Data size is too large.');
        this.toastr.error('Default container for response data is incorrect.');
        throw('Default container for response data is incorrect.');
      }

      await this.logInfo([`6. Default container info:`, '\n\t',
        `url: ${StegomasterConfiguration.DEFAULT_CONTAINER.url};`, '\n\t', `width: ${StegomasterConfiguration.DEFAULT_CONTAINER.width} px;`, '\n\t', 
        `height: ${StegomasterConfiguration.DEFAULT_CONTAINER.height} px;`, '\n\t', `size: ${defaultContainerSize} px.`]);

      return this.loadImage(StegomasterConfiguration.DEFAULT_CONTAINER.url);
    }

    for (let i = 0; i < StegomasterConfiguration.AvailableContainers.length; i++) {
      const container = StegomasterConfiguration.AvailableContainers[i];
      const pixelsCount = container.width * container.height;

      if (pixelsCount > pixelsRequired) {
        await this.logInfo(['6. Suitable container was found:', '\n\t',
          `url: ${container.url};`, '\n\t', `width: ${container.width} px;`, '\n\t',
          `height: ${container.height} px;`, '\n\t', `size: ${container.width * container.height} px.`]);

        return this.loadImage(container.url);
      }
    }

    await this.LogError('Errod: suitable container for entered data not found. Data size is too large.');
    this.toastr.error('Suitable container for entered data not found.');
    throw('Suitable container for entered data not found.');
  }

  public loadImage(url: string): HTMLImageElement {
    let image = new Image();
		image.src = url;
		return image;
  }

  private numberToBinary(data: number): string {
    return data.toString(2).padStart(StegomasterConfiguration.BITS_COUNT, '0');
  }

  private replaceAt(data: string, index: number, replacement: string): string {
    return data.substring(0, index) + replacement + data.substring(index + replacement.length);
  }

  private binaryToString(binary: string[]): string {
    let result = '';
    for (let i = 0; i < binary.length; i++) {
      result += String.fromCharCode(parseInt(binary[i], 2));
    }
    return result;
  }

  public async processRequest(data: string[], image: HTMLImageElement): Promise<StegomasterRequest> {
    await this.logInfo(['7. Operation \'filling the container\' is started. Filling configuration:', '\n\t',
      `skip bits count: ${StegomasterConfiguration.SKIP_BITS_COUNT - 1};`, '\n\t',
      `pixels involved in filling: ${StegomasterConfiguration.ColorsDataHiding.map((i) => RGBA[i]).join(', ')};`, '\n\t',
      `least significant bits (LSB) count: ${StegomasterConfiguration.LSB_COUNT}.`]);
    
    const canvas = document.createElement('canvas');
    const canvasContext = canvas.getContext('2d');

    canvas.style.display = 'none';
    canvas.width = image.width;
    canvas.height = image.height;
    canvasContext.drawImage(image, 0, 0, image.width, image.height);

    const imageData = canvasContext.getImageData(0, 0, canvas.width, canvas.height);
    const binaryData = this.getBinaryData(data);

    await this.logInfo(['8. Image in Base64 format BEFORE filling with data:', '\n', canvas.toDataURL()]);

    for (let i = 0, j = 0; i < imageData.data.length; i += 4 * StegomasterConfiguration.SKIP_BITS_COUNT) {
      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Red) && binaryData.length > j) {
        let redBinary = this.numberToBinary(imageData.data[i]);
        redBinary = this.replaceAt(redBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.substring(j, j + StegomasterConfiguration.LSB_COUNT));
        imageData.data[i] = parseInt(redBinary, 2);
        j += StegomasterConfiguration.LSB_COUNT;
      }

      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Green) && binaryData.length > j) {
        let greenBinary = this.numberToBinary(imageData.data[i + 1]);
        greenBinary = this.replaceAt(greenBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.substring(j, j + StegomasterConfiguration.LSB_COUNT));
        imageData.data[i + 1] = parseInt(greenBinary, 2);
        j += StegomasterConfiguration.LSB_COUNT;
      }

      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Blue) && binaryData.length > j) {
        let blueBinary = this.numberToBinary(imageData.data[i + 2]);
        blueBinary = this.replaceAt(blueBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.substring(j, j + StegomasterConfiguration.LSB_COUNT));
        imageData.data[i + 2] = parseInt(blueBinary, 2);
        j += StegomasterConfiguration.LSB_COUNT;
      }

      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Alpha) && binaryData.length > j) {
        let alphaBinary = this.numberToBinary(imageData.data[i + 3]);
        alphaBinary = this.replaceAt(alphaBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.substring(j, j + StegomasterConfiguration.LSB_COUNT));
        imageData.data[i + 3] = parseInt(alphaBinary, 2);
        j += StegomasterConfiguration.LSB_COUNT;
      }

      if (binaryData.length === j) {
        break;
      }
    }

    canvasContext.putImageData(imageData, 0, 0);

    const base64Data = canvas.toDataURL();
    await this.logInfo(['9. Image in Base64 format AFTER filling with data:', '\n', base64Data]);

    return { data: base64Data } as StegomasterRequest;
  }

  public async processResponse(image: HTMLImageElement): Promise<string[]> {
    const canvas = document.createElement('canvas');
    const canvasContext = canvas.getContext('2d');

    canvas.style.display = 'none';
    canvas.width = image.width;
    canvas.height = image.height;
    canvasContext.drawImage(image, 0, 0, image.width, image.height);

    const imageData = canvasContext.getImageData(0, 0, canvas.width, canvas.height);
    let binaryData: string = '';

    for (let i = 0; i < imageData.data.length; i += 4 * StegomasterConfiguration.SKIP_BITS_COUNT) {
      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Red)) { 
        let redBinary = this.numberToBinary(imageData.data[i]);
        binaryData += redBinary.substring(StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, StegomasterConfiguration.BITS_COUNT);

        if (IsDataEnd()) {
          break;
        }
      }

      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Green)) { 
        let greenBinary = this.numberToBinary(imageData.data[i + 1]);
        binaryData += greenBinary.substring(StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, StegomasterConfiguration.BITS_COUNT);

        if (IsDataEnd()) {
          break;
        }
      }

      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Blue)) { 
        let blueBinary = this.numberToBinary(imageData.data[i + 2]);
        binaryData += blueBinary.substring(StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, StegomasterConfiguration.BITS_COUNT);

        if (IsDataEnd()) {
          break;
        }
      }

      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Alpha)) { 
        let alphaBinary = this.numberToBinary(imageData.data[i + 3]);
        binaryData += alphaBinary.substring(StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, StegomasterConfiguration.BITS_COUNT);

        if (IsDataEnd()) {
          break;
        }
      }
    }

    await this.logInfo(['13. Binary representation of response data:', '\n', binaryData, '\n\t', `bits count: ${binaryData.length}.`]);

    const data: string[] = [];
    let list: string[] = [];

    for (let i = 0; i < binaryData.length; i += StegomasterConfiguration.BITS_COUNT) {
      const byteData = binaryData.substring(i, i + StegomasterConfiguration.BITS_COUNT);

      if (byteData === StegomasterConfiguration.DATA_DELIMETER) {
        data.push(this.binaryToString(list));
        list = [];
        continue;
      } else if (byteData === StegomasterConfiguration.DATA_ENDING) {
        data.push(this.binaryToString(list));
        return data;
      }

      list.push(byteData);
    }
    
    return data;

    function IsDataEnd(): Boolean {
      return binaryData.length >= StegomasterConfiguration.DATA_ENDING.length &&
        binaryData.substring(binaryData.length - StegomasterConfiguration.DATA_ENDING.length, binaryData.length + StegomasterConfiguration.DATA_ENDING.length) === StegomasterConfiguration.DATA_ENDING;
    }
  }

  private waiter(): Promise<void> {
    return new Promise((resolve, _) => {
      setTimeout(() => resolve(), StegomasterInfoConfiguration.INFO_DELAY);
    });
  }

  public async logInfo(info: string[]): Promise<void> {
    if (StegomasterInfoConfiguration.USE_INFO) {
      console.log(...info);
      await this.waiter();
    }
  }

  public async LogError(error: string): Promise<void> {
    if (StegomasterInfoConfiguration.USE_INFO) {
      console.error(error);
      await this.waiter();
    }
  }
}
