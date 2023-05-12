import { RGBA } from "./rgba";

export class StegomasterConfiguration {
  public static readonly CONTAINER_URL: string = '../../assets/stegomaster/container_200x300.jpg';
  public static readonly DATA_DELIMETER: string = '11111111';
  public static readonly DATA_ENDING: string = '11111110';
  public static readonly BITS_COUNT = 8;
  public static readonly LSB_COUNT = 2; // 2 or 4 or 8
  
  public static ColorsDataHiding: Readonly<RGBA[]> = [RGBA.Red, RGBA.Green, RGBA.Blue];
}