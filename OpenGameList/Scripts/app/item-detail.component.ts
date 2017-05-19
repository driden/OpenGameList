import { Component, OnInit } from "@angular/core";
import { Item } from "./item";
import { ItemService } from "./item.service";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
    selector: "item-detail",
    template: `
        <div *ngIf="item" class="item-details">
            <h2>{{item.Title}} - Detail View</h2>
            <ul>
                <li>
                    <label>Title:</label>
                    <input [(ngModel)]="item.Title" placeholder ="Insert the title..."/>
                </li>
                <li>
                    <label>Description:</label>
                    <textarea [(ngModel)]="item.Description" placeholder ="Insert a suitable description..."></textarea>
                </li>
            </ul>
        </div>`,
    styles: [`
        .item-details {
            margin: 5px;
            padding: 5px 10px;
            border: 1px solid black;
            background-color: #ddd;
            width: 300px;
        }
        
        .item-details * {
            vertical-align: middle;            
        }
        .item-details ul li {
            padding: 5px 0;
        }
    `]
})

export class ItemDetailComponent {
    item: Item;

    constructor(
        private itemService: ItemService,
        private router :  Router,
        private activatedRoute: ActivatedRoute) { }

    ngOnInit() {
        console.log("sacando id del params");

        var id = +this.activatedRoute.snapshot.params["id"];

        console.log("Id de la lista " + id);

        if (id) {
            this.itemService.get(id).subscribe(i => this.item = i);
            console.log(this.item);
        } else {
            console.log("Invalid id: routing back to home...");
            this.router.navigate([""]);
        }
    }

}