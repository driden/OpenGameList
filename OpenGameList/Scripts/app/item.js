System.register([], function (exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var Item;
    return {
        setters: [],
        execute: function () {
            Item = class Item {
                constructor(Id, Title, Description) {
                    this.Id = Id;
                    this.Title = Title;
                    this.Description = Description;
                }
            };
            exports_1("Item", Item);
        }
    };
});
//# sourceMappingURL=item.js.map