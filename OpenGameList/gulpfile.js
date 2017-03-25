// Get the modules
var gulp = require('gulp'),
    gp_clean = require('gulp-clean'),
    gp_concat = require('gulp-concat'),
    gp_sourcemaps = require("gulp-sourcemaps"),
    gp_typescript = require("gulp-typescript"),
    gp_uglify = require("gulp-uglify");

// Paths
var srcPaths = {
    app: ['Scripts/app/main.ts', 'Scripts/app/**/*.ts'],
    js: ['Scripts/js/**/*.js']
};

var destPaths = {
    app: 'wwwroot/app/',
    js: 'wwwroot/js/'
};

// Compile, minify and create sourcemaps all Typescript files and place them to 
// wwwroot/app, together with their js.map files
gulp.task('app',['app_clean'], function () {
    return gulp.src(srcPaths.app)
        .pipe(gp_sourcemaps.init())
        .pipe(gp_typescript(require('./tsconfig.json').compilerOptions))
        .pipe(gp_uglify({ mangle: false }))
        .pipe(gp_sourcemaps.write('/'))
        .pipe(gulp.dest(destPaths.app));
});

// Delete wwwroot/app contents
gulp.task('app_clean', function () {
    return gulp.src(dest.app + "*", { read: false })
        .pipe(gp_clean({ force: true }));
});

// Copy all js files from external libraries to wwwroot/js
gulp.task('js', function () {
    return gulp.src(srcPaths.js)
        // .pipe(gp_uglify({ mangle: false })) // Disable Uglify
        // .pipe(gp_concat('all-js.min.js')) // Disable Concat
        .pipe(gulp.dest(destPaths.js));
});

// Delete wwwroot/js contents
gulp.task('js_clean', function () {
    return gulp.src(destPaths.js + "*", { read: false })
        .pipe(gp_clean({ force: true }));
});

// Watch specified files and define what to do upon file changes
gulp.task('watch', function () {
    gulp.watch([srcPaths.app,srcPaths.js],['app','js']);
});

// Global cleanup task
gulp.task('cleanup', ['app_clean', 'js_clean']);

// Default task that launches all the other tasks
gulp.task('default',['app','js','watch']);