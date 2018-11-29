import { Component } from '@angular/core';
import { fadeInOut } from '../../services/animations';


@Component({
    selector: 'agents',
    templateUrl: './agents.component.html',
    styleUrls: ['./agents.component.css'],
    animations: [fadeInOut]
})
export class AgentsComponent {

}
