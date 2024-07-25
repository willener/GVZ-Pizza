import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { APP_CONSTANTS } from './shared/constants';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = APP_CONSTANTS.APP_TITLE;
  pizzaForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.pizzaForm = this.fb.group({
      pizzaDescription: ['', [
        Validators.maxLength(APP_CONSTANTS.MAX_DESCRIPTION),
        Validators.required
      ]],
      pizzaDiameter: [APP_CONSTANTS.DEFAULT_DIAMETER, [
        Validators.min(APP_CONSTANTS.MIN_DIAMETER),
        Validators.max(APP_CONSTANTS.MAX_DIAMETER),
        Validators.required
      ]],
      pizzaBakingTime: [null, [
        Validators.min(APP_CONSTANTS.MIN_BAKING_TIME)
      ]],
      toppings: this.fb.array([])
    });
  }

  get pizzaDescription() {
    return this.pizzaForm.get('pizzaDescription');
  }
  get pizzaDiameter() {
    return this.pizzaForm.get('pizzaDiameter');
  }

  get pizzaBakingTime() {
    return this.pizzaForm.get('pizzaBakingTime');
  }

  get toppings() {
    return this.pizzaForm.get('toppings') as FormArray;
  }

  // Add a new topping filed to the form group.
  addItem() {
    this.toppings.push(this.fb.control('', [Validators.maxLength(APP_CONSTANTS.MAX_DESCRIPTION)]));
  }

  // Removes a topping filed with his index.
  removeItem(index: number) {
    this.toppings.removeAt(index);
  }

  // Send form group.
  onSubmit() {
    console.log(this.pizzaForm.value);
    //TODO Send form to API

    this.clearFrom();
  }

  // Clear all form fields.
  clearFrom() {
    for(var topping in this.toppings) {
      this.toppings.removeAt(0);
    }
    this.pizzaForm.get('pizzaDescription')?.setValue('');
    this.pizzaForm.get('pizzaDiameter')?.setValue(APP_CONSTANTS.MAX_DESCRIPTION);
    this.pizzaForm.get('pizzaBakingTime')?.setValue(null);
  }
}
