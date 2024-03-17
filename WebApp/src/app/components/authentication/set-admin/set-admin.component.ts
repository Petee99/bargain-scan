import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { MatTableDataSource } from '@angular/material/table';
import { User } from 'src/app/models/models'


@Component({
  selector: 'app-set-admin',
  templateUrl: './set-admin.component.html',
  styleUrls: ['./set-admin.component.scss']
})
export class SetAdminComponent implements OnInit {
  displayedColumns: string[] = ['email','role','set_admin', 'delete_user'];
  dataSource: MatTableDataSource<User>;
  users: User[];

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.authService.getAllUsers()
    .subscribe(users => {
        if(users.length > 1){
          this.dataSource = new MatTableDataSource(users);
        }
    })
  }

  applyFilter(ev: Event) {
    const filterValue = (ev.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  getVisibility(role){
    if(role == "Admin"){
      return "collapsed"
    }
  }

  setAdmin(email){
    var retVal = confirm(`Are you sure, you want to set ${email}'s role to admin?`);
    if( retVal == true ) {
      this.authService.setAdmin(email)
      .subscribe(data => {
        if(data){
          let user = this.dataSource.data.find(element => element.email == email);
          user.isAdmin
        }
      });
    } else {
      alert("No changes were made.");
    }
  }

  deleteUser(email){
    var retVal = confirm(`Are you sure, you want to delete ${email}?`);
    if( retVal == true ) {
      this.authService.deleteUser(email)
      .subscribe(data => {
        let user = this.dataSource.data.find(element => element.email == email);
        const index = this.dataSource.data.indexOf(user);
        this.dataSource.data.splice(index, 1);
        this.dataSource._updateChangeSubscription();
      });
    } else {
      alert("No changes were made.");
    }
  }
}
