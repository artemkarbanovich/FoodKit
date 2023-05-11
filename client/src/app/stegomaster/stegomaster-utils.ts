export class StegomasterUtils {
  static loadImage(url: string): HTMLImageElement {
		let image = new Image();
		image.src = url;
		return image;
  }

	static toBinary(data: string): string {
		let result  = "";
  	for (var i = 0; i < data.length; i++) {
      let binaryChar = data[i].charCodeAt(0).toString(2);
			if (binaryChar.length < 8) {
				for (let j = binaryChar.length; j < 8; j++) {
					binaryChar = "0" + binaryChar;
				}
			}
			result += binaryChar;
  	}
		return result;
	}

	static  byteString(num): string {
		if (num < 0 || num > 255 || num % 1 !== 0) {
				throw new Error(num + " does not fit in a byte");
		}
		return ("000000000" + num.toString(2)).substr(-8);
	}
}