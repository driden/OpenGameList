import { Component, OnInit } from "@angular/core";
import { Item } from "./item";
import { ItemService } from "./item.service";

@Component({
    selector: "item-list",
    template: `<h2>Latest Items:</h2>
               <ul class="items">
                    <li *ngFor="let item of items"
	                [class.selected]="item===selectedItem"
	                (click)="onSelect(item)">
	                <span>{{item.Title}}</span>
                </ul>`,
    styles: [`
        ul.items li {
            cursor: pointer;
        }
        ul.items li.selected {
            background-color: #ccc;
        }
    `]
})

export class ItemListComponent implements OnInit {
    selectedItem: Item;
    items: Item[];
    errorMessage: string;

    constructor(private itemService: ItemService) { }

    ngOnInit() {
        this.getLatest();    
    }

    getLatest() {
        this.itemService.getLatest()
            .subscribe(latestItems => this.items = latestItems,
            error => this.errorMessage = <any>error);
    }

    onSelect(item: Item) {
        this.selectedItem = item;
        console.log("item with id " + this.selectedItem.Id + " has been selected.");
    }
}