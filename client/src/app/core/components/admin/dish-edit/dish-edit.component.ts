import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dish-edit',
  templateUrl: './dish-edit.component.html',
  styleUrls: ['./dish-edit.component.scss']
})
export class DishEditComponent implements OnInit {

  constructor(private route: ActivatedRoute, private toastr: ToastrService) { }

  public ngOnInit(): void {
    //TODO: load full dish
    this.route.params.subscribe((params: Params) => {
      const prodId = params['id'];
      console.log(prodId);
   });
  }
}