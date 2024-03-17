import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WebScrapingComponent } from './web-scraping.component';

describe('WebScrapingComponent', () => {
  let component: WebScrapingComponent;
  let fixture: ComponentFixture<WebScrapingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WebScrapingComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WebScrapingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
