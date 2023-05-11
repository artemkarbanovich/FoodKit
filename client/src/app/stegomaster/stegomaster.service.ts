import { Injectable } from '@angular/core';
import { StegomasterConfiguration } from './configuration/stegomaster-configuration';
import { StegomasterRequest } from './DTOs/stegomaster-request';
import { RGBA } from './configuration/rgba';

@Injectable({
  providedIn: 'root'
})
export class StegomasterService {
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

  public loadImage(): HTMLImageElement {
    let image = new Image();
		image.src = StegomasterConfiguration.CONTAINER_URL;
		return image;
  }

  private numberToBinary(data: number): string {
    return data.toString(2).padStart(StegomasterConfiguration.BITS_COUNT, '0');
  }

  private replaceAt(data: string, index: number, replacement: string): string {
    return data.substring(0, index) + replacement + data.substring(index + replacement.length);
  }

  public processImage(data: string[], image: HTMLImageElement): StegomasterRequest {
    const canvas = document.createElement('canvas');
    const canvasContext = canvas.getContext('2d');
      
    canvas.style.display = 'none';
    canvas.width = image.width;
    canvas.height = image.height;
    canvasContext.drawImage(image, 0, 0, image.width, image.height);

    const imageData = canvasContext.getImageData(0, 0, canvas.width, canvas.height);
    const binaryData = this.getBinaryData(data);

    for (let i = 0, j = 0; i < imageData.data.length; i += 4) {
      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Red) && binaryData.length > j) {
        let redBinary = this.numberToBinary(imageData.data[i]);
        redBinary = this.replaceAt(redBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.substring(j, j + StegomasterConfiguration.LSB_COUNT));
        imageData.data[i] = parseInt(redBinary, 2);
        j = j + StegomasterConfiguration.LSB_COUNT;
      }

      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Green) && binaryData.length > j) {
        let greenBinary = this.numberToBinary(imageData.data[i + 1]);
        greenBinary = this.replaceAt(greenBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.substring(j, j + StegomasterConfiguration.LSB_COUNT));
        imageData.data[i + 1] = parseInt(greenBinary, 2);
        j = j + StegomasterConfiguration.LSB_COUNT;
      }

      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Blue) && binaryData.length > j) {
        let blueBinary = this.numberToBinary(imageData.data[i + 2]);
        blueBinary = this.replaceAt(blueBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.substring(j, j + StegomasterConfiguration.LSB_COUNT));
        imageData.data[i + 2] = parseInt(blueBinary, 2);
        j = j + StegomasterConfiguration.LSB_COUNT;
      }

      if (StegomasterConfiguration.ColorsDataHiding.includes(RGBA.Alpha) && binaryData.length > j) {
        let alphaBinary = this.numberToBinary(imageData.data[i + 3]);
        alphaBinary = this.replaceAt(alphaBinary, StegomasterConfiguration.BITS_COUNT - StegomasterConfiguration.LSB_COUNT, binaryData.substring(j, j + StegomasterConfiguration.LSB_COUNT));
        imageData.data[i + 3] = parseInt(alphaBinary, 2);
        j = j + StegomasterConfiguration.LSB_COUNT;
      }

      if (binaryData.length === j) {
        break;
      }
    }

    canvasContext.putImageData(imageData, 0, 0);
    
    return { data: canvas.toDataURL() } as StegomasterRequest;
  }
}
