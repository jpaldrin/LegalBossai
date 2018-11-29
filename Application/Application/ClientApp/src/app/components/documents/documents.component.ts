 
import { Component } from '@angular/core';
import { fadeInOut } from '../../services/animations';

@Component({
    selector: 'documents',
    templateUrl: './documents.component.html',
    styleUrls: ['./documents.component.css'],
    animations: [fadeInOut]
})
export class DocumentsComponent {
}
