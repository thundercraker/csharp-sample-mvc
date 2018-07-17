var webpack = require('webpack');
var awesome = require('awesome-typescript-loader');

module.exports = {
  mode: 'development',
  watch: true,
  entry: "./src/index.tsx",
  output: {
    filename: "bundle.js",
    path: __dirname + "/dist"
  },

  // Enable sourcemaps for debugging webpack's output.
  devtool: "source-map",

  resolve: {
    extensions: [".ts", ".tsx", ".js", ".json"]
  },

  module: {
    rules: [
      // All '.ts', '.tsx' -> 'awesome-typescript-loader'.
      {
        test: /\.tsx?$/, 
        loader: "awesome-typescript-loader",
        options: {
          useCache: true
        }
      },

      // All output '.js' -> 'source-map-loader'.
      { enforce: "pre", test: /\.js$/, loader: "source-map-loader" }
    ]
  },

  // Donot bundle these dependencies, assumes they are available in global scope
  // allows us to make them available via CDN  
  externals: {
    "react": "React",
    "react-dom": "ReactDOM"
  },
  plugins: [
      new webpack.DefinePlugin({
          'process.env.NODE_ENV': JSON.stringify('development'),
          'process.env.DEBUG': JSON.stringify(true)
      }),
      new awesome.CheckerPlugin()
  ]
};