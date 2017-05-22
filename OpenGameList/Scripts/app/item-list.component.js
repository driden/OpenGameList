System.register(["@angular/core", "@angular/router", "./item.service"], function (exports_1, context_1) {
    "use strict";
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var __moduleName = context_1 && context_1.id;
    var core_1, router_1, item_service_1, ItemListComponent;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (item_service_1_1) {
                item_service_1 = item_service_1_1;
            }
        ],
        execute: function () {
            ItemListComponent = class ItemListComponent {
                constructor(itemService, router) {
                    this.itemService = itemService;
                    this.router = router;
                }
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
                    s.subscribe(items => this.items = items, error => this.errorMessage = error);
                }
                getLatest() {
                    this.itemService.getLatest()
                        .subscribe(latestItems => this.items = latestItems, error => this.errorMessage = error);
                }
                onSelect(item) {
                    this.selectedItem = item;
                    console.log("item with id " + this.selectedItem.Id + " has been clicked: loading ItemDetailComponent");
                    this.router.navigate(["item", this.selectedItem.Id]);
                }
            };
            __decorate([
                core_1.Input(),
                __metadata("design:type", String)
            ], ItemListComponent.prototype, "class", void 0);
            ItemListComponent = __decorate([
                core_1.Component({
                    selector: "item-list",
                    template: `<h2>{{title}}</h2>
               <ul class="items">
                    <li *ngFor="let item of items"
	                    [class.selected]="item===selectedItem"
	                    (click)="onSelect(item)">
	                    <span>{{item.Title}}</span>
                    </li>
                </ul>`,
                    styles: [`
        ul.items li {
            cursor: pointer;
        }
        ul.items li.selected {
            background-color: #ccc;
        }
    `]
                }),
                __metadata("design:paramtypes", [item_service_1.ItemService, router_1.Router])
            ], ItemListComponent);
            exports_1("ItemListComponent", ItemListComponent);
        }
    };
});
//# sourceMappingURL=item-list.component.js.map