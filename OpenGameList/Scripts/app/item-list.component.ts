﻿import { Component, Input, OnInit } from "@angular/core";
import { Item } from "./item";
import { ItemService } from "./item.service";

@Component({
    selector: "item-list",
    template: `<h2>{{title}}:</h2>
               <ul class="items">
                    <li *ngFor="let item of items"
	                [class.selected]="item===selectedItem"
	                (click)="onSelect(item)">
	                <span>{{item.Title}}</span>
                </ul>
                <item-detail *ngIf="selectedItem" [item]="selectedItem"></item-detail>`,
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
    @Input() class: string;
    title: string;
    selectedItem: Item;
    items: Item[];
    errorMessage: string;

    constructor(private itemService: ItemService) { }

    ngOnInit() {
        console.log("ItemListComponent instantiated with type " + this.class);
        var s = null;
        switch (this.class) {

            case "most-viewed":
                this.title = "Most Viewed Items";
                s = this.itemService.getMostViewed();
                break;

            case "random":
                this.title = "Random Items";
                s = this.itemService.getRandom();
                break;

            case "latest":
            default:
                this.title = "Latest Items";
                s = this.itemService.getLatest();
        }

        s.subscribe(
            items => this.items = items,
            error => this.errorMessage = <any>error
        );
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