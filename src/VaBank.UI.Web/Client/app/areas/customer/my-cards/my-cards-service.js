(function () {
    'use strict';

    angular
        .module('vabank.webapp')
        .service('myCardsService', myCardsService);

    myCardsService.$inject = ['$resource', 'dataUtil', 'authService', 'securityCodeService', 'transferService'];

    function myCardsService($resource, dataUtil, authService, securityCodeService, transferService) {

        var getUserId = function() {
            return authService.getUser().id;
        };

        var Card = $resource('/api/cards/:cardId', {cardId: '@cardId'}, {
            query: {
                url: '/api/users/:userId/cards',
                isArray: true,
                params: { userId: getUserId }
            },
            updateSettings : {
                url: '/api/cards/:cardId/settings',
                method: 'PUT'
            },
            block: {
                url: '/api/cards/:cardId/block',
                method: 'POST'
            },
            statement: {
                url: '/api/cards/:cardId/statement',
                method: 'GET'
            },
            balance: {
                url: '/api/cards/:cardId/balance/:currencyIsoName',
                method: 'GET'
            },
            costsByPaymentCategory: {
                url: '/api/cards/:cardId/costs',
                method: 'GET',
                isArray: false,
                params: {
                    from: function () {
                        return moment().utc().subtract(3, 'month').startOf('month').toJSON();
                    },
                    to: function () {
                        return moment().utc().toJSON();
                    }
                }
            }
        });
        Card.prototype.isExpired = function() {
            var now = moment().utc().startOf('day');
            var expires = moment.utc(this.expirationDateUtc).startOf('day');
            return expires.isSame(now) || expires.isBefore(now);
        };
        Card.queryAllowed = function() {
            return Card.query().$promise.then(function (cards) {
                return _.chain(cards).where({ blocked: false }).filter(function filterExpired(card) {
                    return !card.isExpired();
                }).value();
            });
        };

        var CardAccount = $resource('/api/accounts/card/:accountNo', { accountNo: '@accountNo' }, {
            statement: {
                url: '/api/accounts/card/:accountNo/statement',
                method: 'GET',
                isArray: false
            }
        });
        CardAccount.query = function () {
            return Card.query().$promise.then(function(cards) {
                return _.chain(cards)
                    .groupBy('accountNo')
                    .map(function (value, key) {
                        return {
                            accountNo: key,
                            balance: value[0].balance,
                            currency: value[0].currency,
                            cards: value
                        }
                    }).value();
            });
        };

        var SecurityCode = securityCodeService;

        var Transfer = transferService.Transfer;

        return {
            Card: Card,
            CardAccount: CardAccount,
            SecurityCode: SecurityCode,
            Transfer: Transfer
        };

    }
})();