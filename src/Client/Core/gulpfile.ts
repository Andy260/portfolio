const { src, dest } = require("gulp");
const del = require("del");
const filelog = require("gulp-filelog");
const gulpif = require("gulp-if");
const sourcemaps = require("gulp-sourcemaps");
const ts = require("gulp-typescript");
const uglify = require("gulp-uglify");

let isProduction: boolean = process.env.NODE_ENV === "production"; 

// Compiles all Typescript files
function compileTypescript(): void
{
    // Create Typescript project
    let tsProject = ts.createProject("tsconfig.json");
    // Create pipe for compilation
    let tsResult = src(["**/*.ts", "!node_modules/**", "!gulpfile.ts", "!wwwroot/**"])
        .pipe(filelog())
        .pipe(gulpif(!isProduction, sourcemaps.init()))
        .pipe(tsProject());

    return tsResult.js.pipe(gulpif(!isProduction, sourcemaps.write("./")))
        .pipe(gulpif(isProduction, uglify()))
        .pipe(dest("./"));
}

// Removes all compiled JavaScript files
function cleanTypescript(): void
{
    return del(["**/*.js" , "**/*js.map", "!node_modules/**", "!gulpfile.js", "!gulpfile.ts"]);
}

exports.compileTypescript = compileTypescript;
exports.clean = cleanTypescript;
