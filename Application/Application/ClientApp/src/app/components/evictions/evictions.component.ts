 
import { Component } from '@angular/core';
import { fadeInOut } from '../../services/animations';

@Component({
    selector: 'evictions',
    templateUrl: './evictions.component.html',
    styleUrls: ['./evictions.component.css'],
    animations: [fadeInOut]
})
export class EvictionsComponent {
}
