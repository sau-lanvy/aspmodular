/// <binding Clean='clean-modules' AfterBuild='copy-modules' />
"use strict";

var gulp = require('gulp'),
    clean = require('gulp-clean'),
    glob = require("glob");

var paths = {
    devModules: "../Modules/",
    hostModules: "./Modules/"
};

var modules = loadModules();

gulp.task('clean-modules', function () {
    return gulp.src([paths.hostModules + '*'], { read: false })
    .pipe(clean());
});

gulp.task('copy-modules', ['clean-modules'], function () {
    //console.log(modules);
    modules.forEach(function (module) {
        gulp.src([paths.devModules + module.fullName + '/Views/**/*.*', paths.devModules + module.fullName + '/wwwroot/**/*.*'], { base: module.fullName })
            .pipe(gulp.dest(paths.hostModules + module.fullName));
        gulp.src(paths.devModules + module.fullName + '/bin/Debug/netstandard1.6/**/*.*')
            .pipe(gulp.dest(paths.hostModules + module.fullName + '/bin'));
    });
});

function loadModules() {
    var moduleManifestPaths,
        modules = [];

    moduleManifestPaths = glob.sync(paths.devModules + 'AspModular.Module.*/module.json', {});
    
    moduleManifestPaths.forEach(function (moduleManifestPath) {
        var moduleManifest = require(moduleManifestPath);
        modules.push(moduleManifest);
    });

    return modules;
}
