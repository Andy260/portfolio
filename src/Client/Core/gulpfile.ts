const { src, dest } = require("gulp");
const del = require("del");
const sourcemaps = require("gulp-sourcemaps");
const ts = require("gulp-typescript");

// Compiles all Typescript files
function compileTypescript(): void
{
    // Create Typescript project
    let tsProject = ts.createProject("tsconfig.json");
    // Create pipe for compilation
    let tsResult = src(["**/*.ts", "!node_modules/**", "!gulpfile.ts", "!wwwroot/**"])
        .pipe(sourcemaps.init())
        .pipe(tsProject());

    return tsResult.js.pipe(sourcemaps.write("./"))
        .pipe(dest("./"));
}

// Removes all compiled JavaScript files
function cleanTypescript(): void
{
    return del(["**/*.js", "!node_modules/**", "!gulpfile.js", "!gulpfile.ts"]);
}

exports.compileTypescript = compileTypescript;
exports.clean = cleanTypescript;
