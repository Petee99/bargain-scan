import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { ParsingService } from 'src/app/services/parsing.service';

@Component({
  selector: 'app-sensor-data',
  templateUrl: './sensor-data.component.html',
  styleUrls: ['./sensor-data.component.scss']
})
export class SensorDataComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;

  constructor( private authService: AuthService, private parsingService: ParsingService) { }

  ngOnInit(): void {
  }

  onFileChange(event: any) {
    let file = event.target.files[0];

    // Check if the file is a CSV
    if (file.type !== "text/csv" && file.type !== "application/json") {
        alert("Please upload a CSV or JSON file.");
        return;
    }

    this.parsingService.parseFile(file);
  }

  upload(){
    this.parsingService.sendData();
    this.fileInput.nativeElement.value = "";
  }
}
