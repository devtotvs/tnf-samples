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
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var organizationUnit_service_1 = require("./organizationUnit.service");
var hub_api_1 = require("../utils/hub-api");
var OrganizationThfLookupService = /** @class */ (function () {
    function OrganizationThfLookupService(organizationService) {
        this.organizationService = organizationService;
    }
    OrganizationThfLookupService.prototype.getFilteredData = function (filter, page, pageSize) {
        return this.organizationService.getQueryByTenant(new hub_api_1.HubApiQueryString({
            page: page,
            pageSize: pageSize,
            filter: [{ key: "name", value: filter }]
        }), this.tenantId);
    };
    OrganizationThfLookupService.prototype.getObjectByValue = function (value) {
        return this.organizationService.get(value);
    };
    OrganizationThfLookupService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [organizationUnit_service_1.OrganizationService])
    ], OrganizationThfLookupService);
    return OrganizationThfLookupService;
}());
exports.OrganizationThfLookupService = OrganizationThfLookupService;
//# sourceMappingURL=organizationUnit.thflookup.service.js.map