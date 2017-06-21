System.register(["@angular/core", "@angular/router"], function (exports_1, context_1) {
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
    var core_1, router_1, AppComponent;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            }
        ],
        execute: function () {
            AppComponent = class AppComponent {
                constructor(router) {
                    this.router = router;
                    this.title = "OpenGameList";
                }
                isActive(data) {
                    return this.router.isActive(this.router.createUrlTree(data), true);
                }
            };
            AppComponent = __decorate([
                core_1.Component({
                    selector: "opengamelist",
                    template: `
<nav class="navbar navbar-default navbar-fixed-top">
	<div class="container-fluid">
		<input type="checkbox" id="navbar-toggle-cbox"/>
		<div class="navbar-header">
			<label for="navbar-toggle-cbox" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
				<span class="sr-only">Toggle navigation</span>
				<span class="icon-bar"></span>
				<span class="icon-bar"></span>
				<span class="icon-bar"></span>
			</label>
			<a>
				<img alt="logo" src="/img/logo.svg"/>
			</a>
		</div>
		<div class="collapse navbar-collapse" id="navbar">
			<ul class="nav navbar-nav">
				<li [class.active]="isActive([''])"><a class="home" [routerLink]="['']">Home</a></li>
				<li [class.active]="isActive(['about'])"><a class="about" [routerLink]="['about']">About</a></li>
				<li [class.active]="isActive(['login'])"><a class="login" [routerLink]="['login']">Login</a></li>
				<li [class.active]="isActive(['item/edit',0])"><a class="add" [routerLink]="['item/edit',0]">Add New</a></li>
			</ul>
		</div>
	</div>
</nav>
<h1 class="header">{{title}}</h1>
<div class="main-container">
	<router-outlet></router-outlet>
</div>`
                }),
                __metadata("design:paramtypes", [router_1.Router])
            ], AppComponent);
            exports_1("AppComponent", AppComponent);
        }
    };
});
//# sourceMappingURL=app.component.js.map