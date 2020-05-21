import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'leaguePipe'
})
export class LeaguePipe implements PipeTransform {

  transform(league: number): string {
    switch (league) {
      case 0:
        return 'MLB';
        break;
      case 1:
        return 'AAA';
        break;
      case 2:
        return 'AA';
        break;
      default:
        return 'U';
    }
  }
}
