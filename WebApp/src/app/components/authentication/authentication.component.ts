import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss']
})
export class AuthenticationComponent implements OnInit {

  constructor(private authService:AuthService, private router:Router) { }

  ngOnInit(): void {
    this.authService.isLoggedIn().then(
      () => {},
      response=> {
        if(response.error.text == 'User' || response.error.text == 'Admin'){
          this.authService.navToTasks();
        }        
      });
  }

  onLoginButtonClick(username: string, password:string){
    this.authService.login(username, password)
  }

}
