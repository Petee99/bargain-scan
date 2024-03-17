import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['../authentication.component.scss']
})
export class RegistrationComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    
  }

  onRegButtonClick(email: string, username: string, password:string) {
    if(password.length<9){
      alert("Your password needs to be at least 8 characters long")
    }
    else{
      this.authService.register(email, username, password).subscribe((res: HttpResponse<any>) => {
        console.log(res);
      })
    }
  }

}
