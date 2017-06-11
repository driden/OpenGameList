System.register(["@angular/core", "./item", "./item.service", "@angular/router"], function (exports_1, context_1) {
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
    var core_1, item_1, item_service_1, router_1, ItemDetailComponent;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (item_1_1) {
                item_1 = item_1_1;
            },
            function (item_service_1_1) {
                item_service_1 = item_service_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            }
        ],
        execute: function () {
            ItemDetailComponent = class ItemDetailComponent {
                constructor(itemService, router, activatedRoute) {
                    this.itemService = itemService;
                    this.router = router;
                    this.activatedRoute = activatedRoute;
                }
                ngOnInit() {
                    console.log("sacando id del params");
                    var id = +this.activatedRoute.snapshot.params["id"];
                    console.log("Id de la lista " + id);
                    if (id) {
                        this.itemService.get(id).subscribe(i => this.item = i);
                    }
                    else if (id == 0) {
                        console.log("id is 0; adding new item...");
                        this.item = new item_1.Item(0, "New Item", null);
                    }
                    else {
                        console.log("Invalid id: routing back to home...");
                        this.router.navigate([""]);
                    }
                }
                onInsert(item) {
                    this.itemService.add(item).subscribe((data) => {
                        this.item = data;
                        console.log("Item " + this.item.Id + " has been added.");
                        this.router.navigate([""]);
                    }, (error) => console.log(error));
                }
                onUpdate(item) {
                    this.itemService.update(item).subscribe((data) => {
                        this.item = data;
                        console.log("Item " + this.item.Id + " has been updated.");
                        this.router.navigate([""]);
                    }, (error) => console.log(error));
                }
                onDelete(item) {
                    var id = item.Id;
                    this.itemService.delete(id).subscribe((data) => {
                        console.log("Item " + this.item.Id + " has been deleted.");
                        this.router.navigate([""]);
                    }, (error) => console.log(error));
                }
                onBack() { this.router.navigate([""]); }
            };
            ItemDetailComponent = __decorate([
                core_1.Component({
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
            <div *ngIf="item.Id == 0" class="commands insert">
                <input type="button" value="Save" (click)="onInsert(item)"/>
                <input type="button" value="Cancel" (click)="onBack()"/>
            </div>
            <div *ngIf="item.Id != 0" class="commands update">
                <input type="button" value="Update" (click)="onUpdate(item)"/>
                <input type="button" value="Delete" (click)="onDelete(item)"/>
                <input type="button" value="Back" (click)="onBack()"/>
            </div>
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
                }),
                __metadata("design:paramtypes", [item_service_1.ItemService,
                    router_1.Router,
                    router_1.ActivatedRoute])
            ], ItemDetailComponent);
            exports_1("ItemDetailComponent", ItemDetailComponent);
        }
    };
});
//# sourceMappingURL=item-detail.component.js.map