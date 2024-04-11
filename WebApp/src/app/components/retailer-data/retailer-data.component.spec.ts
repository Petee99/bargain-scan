import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RetailerDataComponent } from './retailer-data.component';

describe('RetailerDataComponent', () => {
  let component: RetailerDataComponent;
  let fixture: ComponentFixture<RetailerDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RetailerDataComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RetailerDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
