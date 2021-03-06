import { NgxMatDateFormats } from "@angular-material-components/datetime-picker";

export const DATE_TIME_FORMATS: NgxMatDateFormats = {
    parse: {
      dateInput: 'l, LTS'
    },
    display: {
      dateInput: 'DD.MM.YYYY  HH:mm',
      monthYearLabel: 'MMM YYYY',
      dateA11yLabel: 'LL',
      monthYearA11yLabel: 'MMMM YYYY',
    }
  };