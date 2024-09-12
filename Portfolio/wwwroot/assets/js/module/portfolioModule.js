var PortfolioModule = angular.module('PortfolioModule', ['ngRoute', 'ngMessages', 'datatables', 'kendo.directives', 'cfp.hotkeys', 'ngHandsontable', 'angularjs-dropdown-multiselect']).run(['$rootscope', function ($rootscope) {

}]);
PortfolioModule.config(function ($routeProvider, $locationProvider) {
    $routeProvider.when('/',
        {
            templateUrl: function () {
                return 'Portfolio/Home/Home';
            }
        });
    $routeProvider.when('/Login',
        {
            templateUrl: function () {
                return '/Portfolio/Home/Login';
            }
        });
    $routeProvider.when('/About',
        {
            templateUrl: function () {
            return 'Portfolio/Home/About';
            }
        });
    $routeProvider.when('Contact', {
        templateUrl: function () {
            return 'Portfolio/Home/Contact';
        }
    });
})
