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


                $scope.RehberOgretmeni = { label: "-select-"};
                $scope.SinifOgretmeni = { label:"-select-"};
                $scope.Hobi = [];
                $scope.bolum = { label:'-select-'};
                $scope.AdSoyad = "";
                $scope.Ogrenci = {};
                $scope.SilinanOgrenci = {};

                $scope.HobiFilter = [];
                $scope.AdSoyadFilter = "";
                $scope.SinifOgretmeniFilter = {};

                $scope.SilinmeNedini = "";

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
                        xsrf += "&SortByAdSonad=" + $scope.AdSoyadSort;
                    }
                    if ($scope.bolumSort != 0) {
                        xsrf += "&SortByBolum=" + $scope.bolumSort;
                    }
                    if ($scope.SinifOgretmeniSort != 0) {
                        xsrf += "&SortBySinifOgretmen=" + $scope.SinifOgretmeniSort;
                    }
                    if ($scope.SinifOgretmeniFilter.id != undefined && $scope.SinifOgretmeniFilter.id != 0) {
                        xsrf += "&SinifOgretmeni=" + $scope.SinifOgretmeniFilter.id;
                    }
                    if ($scope.AdSoyadFilter != "" ) {
                        xsrf += "&Ara=" + $scope.AdSoyadFilter;
                    }
                    if ($scope.HobiFilter.length > 0) {
                        for (let row in $scope.HobiFilter) {
                            xsrf += "&Hobiler[" + row+"]=" + $scope.HobiFilter[row].id
                        }
                      
                    }

                    $http({
                        method: 'POST',
                        url: '/GetOgrenciler',
                        data: xsrf,
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
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

                $scope.Sil = function ($row) {
                    var xsrf = "Id=" + $row.id;
                    xsrf += "&Reason=" + $scope.SilinmeNedini;

                    $http({
                        method: 'POST',
                        url: '/Sil',
                        data: xsrf,
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    }).then(
                        function (response) {
                            console.log("Done!")
                            $scope.SilinmeNedini = "";
                            $scope.SilinanOgrenci = {};
                            $scope.showSilModal = false;

                            $timeout(function () {

                                $scope.GetOgrenciler();

                            }, 2000);
                        },
                        function (response) {
                            console.log('اخطار Connection!');
                            return false;
                        }
                    );
                }

                $scope.Vazgec = function () {

                    $scope.SilinmeNedini = '';
                    $scope.SilinanOgrenci = {};
                    $scope.showSilModal = false;
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
                            $scope.RehberOgretmeni = $scope.Ogrenci.rehberOgretmeni;
                            $scope.SinifOgretmeni = $scope.Ogrenci.sinifOgretmeni;
                            $scope.Hobi = $scope.Ogrenci.hobilar;
                            $scope.bolum = $scope.Ogrenci.bolum;
                            $scope.AdSoyad = $scope.Ogrenci.nameFamily;
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

                    $scope.SinifOgretmeniSort = 0;
                    $scope.bolumSort = 0;

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
                    $scope.bolumSort = 0;
                    $scope.AdSoyadSort = 0;

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

                    $scope.SinifOgretmeniSort = 0;
                    $scope.AdSoyadSort = 0;

                    $scope.GetOgrenciler();
                };

                $scope.showModal = false;
                $scope.YeniOgrenci = function () {
                    $scope.showModal = !$scope.showModal;
                };

                $scope.showSilModal = false;
                $scope.SilOgrenci = function ($row) {
                    $scope.showSilModal = !$scope.showSilModal;
                    $scope.SilinanOgrenci = $row;
                };

                $scope.setBolum = function ($val) {
                    $scope.bolum = $val;
                };
                $scope.setRehberOgretmeni = function ($val) {
                    $scope.RehberOgretmeni = $val;
                };
                $scope.setSinifOgretmeni = function ($val) {
                    $scope.SinifOgretmeni = $val;
                };
                $scope.setHobi = function ($val) {
                    $scope.Hobi.push($val);
                };



                $scope.setSinifOgretmeniFilter = function ($val) {
                    $scope.SinifOgretmeniFilter = $val;

                };
                $scope.setHobiFilter = function ($val) {
                    $scope.HobiFilter.push($val);
                };

                $scope.kaydetSubmit = function () {
                    console.log("$ogrenci", 'd');
                    if ($scope.RehberOgretmeni != $scope.Ogrenci.rehberOgretmeni
                        || $scope.SinifOgretmeni != $scope.Ogrenci.sinifOgretmeni
                        || $scope.Hobi != $scope.Ogrenci.hobilar
                        || $scope.bolum != $scope.Ogrenci.bolum
                        || $scope.AdSoyad != $scope.Ogrenci.nameFamily) {

                        $scope.Ogrenci.rehberOgretmeni = $scope.RehberOgretmeni;
                        $scope.Ogrenci.sinifOgretmeni = $scope.SinifOgretmeni;
                        $scope.Ogrenci.hobilar = $scope.Hobi;
                        $scope.Ogrenci.bolum = $scope.bolum;
                        $scope.Ogrenci.nameFamily = $scope.AdSoyad;

                        $scope.Save($scope.Ogrenci);
                    }


                };

                $scope.Save = function ($ogrenci) {
                    console.log("$ogrenci", $ogrenci);
                }

                $scope.clearHobiFilter = function () {
                    $scope.HobiFilter = [];
                };
                $scope.clearSinifOgretmeniFilter = function () {
                    $scope.SinifOgretmeniFilter = { };
                };

               

            }]);
})();
