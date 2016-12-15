"use strict";
var azureMobileApps = require('azure-mobile-apps');
var table = azureMobileApps.table();
table.access = 'authenticated';
table.insert.access = 'authenticated';
table.read.access = 'authenticated';
table.update.access = 'authenticated';
table.delete.access = 'authenticated';
table.perUser = true;
table.dynamicSchema = false;
table.columns = {
    name: "string",
    userId: "string"
};
module.exports = table;
//# sourceMappingURL=user.js.map