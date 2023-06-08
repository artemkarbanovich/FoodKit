import { RGBA } from "./rgba";
import { StegomasterContainer } from "./stegomaster-containers";

export class StegomasterConfiguration {
  public static readonly USE_INFO = true;
  public static readonly INFO_DELAY = 300;
  public static readonly USE_DEFAULT_CONTAINER = false;
  public static readonly DATA_DELIMETER: string = '11111111';
  public static readonly DATA_ENDING: string = '11111110';
  public static readonly BITS_COUNT = 8;
  public static readonly SKIP_BITS_COUNT = 5 + 1; // should be more or equals 1
  public static readonly LSB_COUNT = 8; // 2 or 4 or 8
  
  public static ColorsDataHiding: Readonly<RGBA[]> = [RGBA.Green];

  public static AvailableContainers: Readonly<StegomasterContainer[]> = [
    { url: '../../assets/stegomaster/container_10x10.jpg', width: 10, height: 10 },
    { url: '../../assets/stegomaster/container_100x100.jpg', width: 100, height: 100 }, 
    { url: '../../assets/stegomaster/container_365x261.jpg', width: 365, height: 261 }, 
    { url: '../../assets/stegomaster/container_550x680.jpg', width: 550, height: 680 },
    { url: '../../assets/stegomaster/container_1000x664.jpg', width: 1000, height: 664 }, 
    { url: '../../assets/stegomaster/container_1560x1440.jpg', width: 1560, height: 1440 }, 
    { url: '../../assets/stegomaster/container_1920x1080.jpg', width: 1920, height: 1080 },
    { url: '../../assets/stegomaster/container_3600x2085.jpg', width: 3600, height: 2085 },
  ];
  
  public static readonly DEFAULT_CONTAINER: StegomasterContainer = this.AvailableContainers[6];
}