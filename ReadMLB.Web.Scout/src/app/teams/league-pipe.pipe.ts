import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'leaguePipe'
})
export class LeaguePipePipe implements PipeTransform {

  transform(league: number): string {
    switch (league) {
      case 0:
        return 'majors';
        break;
      case 1:
        return 'AAA';
        break;
      case 2:
        return 'AA';
        break;
    }
  }
}
