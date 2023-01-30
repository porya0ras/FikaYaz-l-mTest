(function () {
    'use strict';

    angular
        .module('app.controllers', [])
        .controller('HomeCtrl', [
            '$scope',
            '$http',
            '$window',
            '$timeout',
            function ($scope, $http, $window, $timeout) {
                $scope.page = 1;
                $scope.editedId = 0;

                $scope.SinifOgretmenler = [];
                $scope.RehberOgretmenler = [];
                $scope.Hobiler = [];
                $scope.bolumler = [];
                $scope.Ogrenciler = [];
                $scope.OgrencilerCount = 0;


                $scope.RehberOgretmeni = {};
                $scope.SinifOgretmeni = {};
                $scope.Hobi = {};
                $scope.bolum = {};
                $scope.AdSoyad = "";
                $scope.Ogrenci = {};

                $scope.HobiFilter = {};
                $scope.AdSoyadFilter = "";
                $scope.SinifOgretmeniFilter = {};

                $scope.bolumSort = 0;
                $scope.AdSoyadSort = 0;
                $scope.SinifOgretmeniSort = 0;

                $scope.init = function () {
                    console.log("init");
                    $scope.GetSinifOgretmenler();
                    $scope.GetRehberOgretmenler();
                    $scope.GetHobiler();
                    $scope.Getbolumler();

                    $scope.GetOgrenciler();
                };

                $scope.GetSinifOgretmenler = function () {
                    $http
                        .get('/GetSinifOgretmenler')
                        .then(function (response) {
                            $scope.SinifOgretmenler = response.data;
                        });
                };

                $scope.GetRehberOgretmenler = function () {
                    $http
                        .get('/GetRehberOgretmenler')
                        .then(function (response) {
                            $scope.RehberOgretmenler = response.data;
                        });
                };

                $scope.GetHobiler = function () {
                    $http
                        .get('/GetHobiler')
                        .then(function (response) {
                            $scope.Hobiler = response.data;
                        });
                };

                $scope.Getbolumler = function () {
                    $http
                        .get('/Getbolumler')
                        .then(function (response) {
                            $scope.bolumler = response.data;
                        });
                };

                $scope.GetOgrenciler = function () {
                    var xsrf = "page=" + $scope.page;

                    if ($scope.AdSoyadSort!=0) {
                        xsrf += "SortByAdSonad=" + $scope.AdSoyadSort;
                    }
                    if ($scope.bolumSort != 0) {
                        xsrf += "SortByBolum=" + $scope.bolumSort;
                    }
                    if ($scope.SinifOgretmeniSort != 0) {
                        xsrf += "SortBySinifOgretmen=" + $scope.SinifOgretmeniSort;
                    }

                    $http({
                        method: 'POST',
                        url: '/GetOgrenciler',
                        data: xsrf,
                        traditional: true,
                    }).then(
                        function (response) {
                            $scope.Ogrenciler = response.data.searchRes;
                            $scope.OgrencilerCount = response.data.count;
                        },
                        function (response) {
                            console.log('اخطار Connection!');
                            return false;
                        }
                    );
                };

                $scope.Detay = function ($data) {
                    $scope.editedId = $data.id;
                    $scope.GetOgrenci($scope.editedId);
                };

                $scope.GetOgrenci = function ($Id) {
                    $http
                        .get('/GetOgrenci/' + $Id)
                        .then(function (response) {
                            $scope.Ogrenci = response.data;
                            $scope.RehberOgretmeni = $scope.Ogrenci.RehberOgretmeni;
                            $scope.SinifOgretmeni = $scope.Ogrenci.SinifOgretmeni;
                            $scope.Hobi = $scope.Ogrenci.Hobilar;
                            $scope.bolum = $scope.Ogrenci.Bolum;
                            $scope.AdSoyad = $scope.Ogrenci.NameFamily;
                        });
                };

                $scope.ChangePage = function ($page) {
                    $scope.page = $page;
                    $scope.GetOgrenciler();
                }

                $scope.ChangeAdSoyadSort = function () {
                    if ($scope.AdSoyadSort == 0) {
                        $scope.AdSoyadSort = 1;
                    }
                    else {
                        if ($scope.AdSoyadSort == 1) {
                            $scope.AdSoyadSort = -1;
                        }
                        else {
                            $scope.AdSoyadSort = 1;
                        }
                    }
                    $scope.GetOgrenciler();
                };
                $scope.ChangeSinifOgretmeniSort = function () {
                    if ($scope.SinifOgretmeniSort == 0) {
                        $scope.SinifOgretmeniSort = 1;
                    }
                    else {
                        if ($scope.SinifOgretmeniSort == 1) {
                            $scope.SinifOgretmeniSort = -1;
                        }
                        else {
                            $scope.SinifOgretmeniSort = 1;
                        }
                    }
                    $scope.GetOgrenciler();
                };
                $scope.ChangebolumSort = function () {
                    if ($scope.bolumSort == 0) {
                        $scope.bolumSort = 1;
                    }
                    else {
                        if ($scope.bolumSort == 1) {
                            $scope.bolumSort = -1;
                        }
                        else {
                            $scope.bolumSort = 1;
                        }
                    }
                    $scope.GetOgrenciler();
                };

                $scope.showModal = false;
                $scope.YeniOgrenci = function () {
                    $scope.showModal = !$scope.showModal;
                };

            }]);
})();
