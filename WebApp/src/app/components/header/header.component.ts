import { Component, OnInit, AfterViewInit} from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service'


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, AfterViewInit {

  constructor(private authService:AuthService, private router:Router) { }

  ngAfterViewInit(): void {
    
  }

  ngOnInit(): void {
    this.authService.isLoggedIn().then(
      () => {},
      response=> {
        if(response.error.text == 'User'){
          document.getElementById("topnav").innerHTML = document.getElementById("userTopnav").innerHTML;
          document.getElementById("profile").addEventListener('click', () => this.authService.signout());
        }
        else if(response.error.text == 'Admin'){
          document.getElementById("topnav").innerHTML = document.getElementById("adminTopnav").innerHTML;
          document.getElementById("profile").addEventListener('click', () => this.authService.signout());
        }
        else if(this.router.url != '/login' && this.router.url != '/register') {
          document.getElementById("topnav").innerHTML = document.getElementById("baseTopnav").innerHTML;
        }
      },
    ).catch(error => {if(this.router.url != '/login' && this.router.url != '/register') {
      document.getElementById("topnav").innerHTML = document.getElementById("baseTopnav").innerHTML;
    }}) 
  }
}
