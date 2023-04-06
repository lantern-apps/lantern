import { defineConfig } from "tsup";

export default defineConfig([
  {
    entry: {
      lantern: "src/index.ts",
    },
    outDir: "dist",
    format: ["cjs", "esm"],
    dts: true,
    sourcemap: true,
    clean: true,
  },
  {
    entry: {
      lantern: "src/builtin.ts",
    },
    outDir: "builtin",
    format: ["iife"],
    clean: true,
    minify: true,
  },
]);
