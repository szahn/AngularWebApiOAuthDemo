var gulp = require('gulp');
var concat = require('gulp-concat');
var ngAnnotate = require('gulp-ng-annotate');

/** Merge all application js files into a single app.js */
gulp.task('minifyApp', function(){
	/*Merge all js components */
	gulp.src(['app/**/*.js'])
		.pipe(concat('app.js'))
		//Perform some static analysis on the bundles angular js file
        .pipe(ngAnnotate())
		.pipe(gulp.dest('.'));

});


gulp.task('default', ["minifyApp"]);