System.register(["@angular/core"], function (exports_1, context_1) {
    "use strict";
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __moduleName = context_1 && context_1.id;
    var core_1, AppComponent;
    return {
        setters: [
            function (core_1_1) {
                core_1 = core_1_1;
            }
        ],
        execute: function () {
            AppComponent = class AppComponent {
                constructor() {
                    this.title = "OpenGameList";
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
				<li><a class="home" [routerLink]="['']">Home</a></li>
				<li><a class="about" [routerLink]="['about']">About</a></li>
				<li><a class="login" [routerLink]="['login']">Login</a></li>
				<li><a class="add" [routerLink]="['item/edit',0]">Add New</a></li>
			</ul>
		</div>
	</div>
</nav>
<h1 class="header">{{title}}</h1>
<div class="main-container">
	<router-outlet></router-outlet>
</div>`
                })
            ], AppComponent);
            exports_1("AppComponent", AppComponent);
        }
    };
});
//# sourceMappingURL=app.component.js.map