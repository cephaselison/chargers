'use strict';

const gulp = require('gulp');
const connect = require('gulp-connect');

gulp.task('server', function() {
    connect.server({
        root: 'src/',
        port: 5050
    });
});

gulp.task('watch', function() {
    gulp.watch('src', function() {
        connect.reload();
    });
});

gulp.task('default', gulp.parallel('watch', 'server', function() {

}));
