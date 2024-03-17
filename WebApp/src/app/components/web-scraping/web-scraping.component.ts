import { Component, OnInit } from '@angular/core';
import { ScrapeRequest, ScrapeRequestType } from 'src/app/models/models';
import { AuthService } from 'src/app/services/auth.service';
import { ScrapeRequestService } from 'src/app/services/web-scraping.service';

@Component({
  selector: 'app-web-scraping',
  templateUrl: './web-scraping.component.html',
  styleUrls: ['./web-scraping.component.scss']
})
export class WebScrapingComponent implements OnInit {

  constructor( private authService: AuthService, private scrapeRequestService: ScrapeRequestService) { }

  ngOnInit(): void {
  }

  requestScraping(requestInterval: number){

    let request = new ScrapeRequest();

    if(requestInterval > 0){
      request.RequestType = ScrapeRequestType.Timed;
    }
    else{
      request.RequestType = ScrapeRequestType.Immediate;
    }

    request.Days = requestInterval;

    this.scrapeRequestService.requestService(request)
  }

}
