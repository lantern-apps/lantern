import path from "path";
import glob from "glob";
import { defineConfig } from "vite";

export default defineConfig({
  root: "src",
  build: {
    outDir: "../dist",
    emptyOutDir: true,
    minify: false,
    rollupOptions: {
      input: glob.sync("src/**/index.html").reduce(
        (input, file) => ({
          ...input,
          [path
            .relative(
              "src",
              file.slice(0, file.length - path.extname(file).length)
            )
            .replaceAll("\\", "/")]: file.replaceAll("\\", "/"),
        }),
        {}
      ),
      output: {
        entryFileNames: "[name].js",
        chunkFileNames: "[name].js",
      },
    },
  },
});
