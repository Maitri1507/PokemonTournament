import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-error-page',
  standalone: true,
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.css']
})
export class ErrorPageComponent {
  constructor(private router: Router) {}

  tryAgain(): void {
    const returnUrl = history.state?.returnUrl || '/';
    this.router.navigateByUrl(returnUrl);
  }
}