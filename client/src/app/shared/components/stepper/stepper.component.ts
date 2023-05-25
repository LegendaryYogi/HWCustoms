import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-stepper',
  templateUrl: './stepper.component.html',
  styleUrls: ['./stepper.component.scss'],
  providers: [{provide: CdkStepper, useExisting: StepperComponent}]   //232
})
export class StepperComponent extends CdkStepper implements OnInit {
    @Input() linearModeSelected = true;   //passing from checkout component/html

    ngOnInit(): void {
      this.linear = this.linearModeSelected;  //232
    }

    onClick(index: number) {
      this.selectedIndex = index;
    }
}
