import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetAdminComponent } from './set-admin.component';

describe('SetAdminComponent', () => {
  let component: SetAdminComponent;
  let fixture: ComponentFixture<SetAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SetAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SetAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
