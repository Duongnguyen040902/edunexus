# Assets Directory

Welcome to the assets directory of our project! This directory is crucial for organizing the static files used throughout the project. It includes images, styles, and potentially other resources like fonts or icons. Below, you'll find guidelines on how to use the `image` and `style` subdirectories effectively.

## Structure

The assets directory is divided into two main subdirectories:

- `image`: Contains all the graphical resources used in the project, such as icons, logos, and background images.
- `style`: Houses the SCSS/CSS files that define the visual appearance of the project. The main file here is `app.scss`, which serves as the entry point for all style definitions.

### Image Directory

The `image` directory is intended to store all the visual assets. To keep this directory organized, consider categorizing images into subfolders (e.g., `icons`, `backgrounds`, etc.) if your project has a wide variety of images.

#### Best Practices

- **Optimization**: Before adding images to this directory, ensure they are optimized for the web to improve loading times and overall performance.
- **Naming Convention**: Use clear and descriptive names for your image files, using dashes (`-`) to separate words (e.g., `logo-dark-mode.png`).

### Style Directory

The `style` directory contains the SCSS files. `app.scss` is the primary stylesheet, where global styles are defined and other SCSS files are imported.

#### Best Practices

- **Modularity**: Utilize SCSS's features to create modular and reusable styles. Break down your styles into partials (e.g., `_buttons.scss`, `_typography.scss`) and import them into `app.scss`.
- **Variables**: Make use of SCSS variables for colors, font sizes, and other values that are used in multiple places. This makes it easier to maintain and update your styles.

## Usage

### Images

To use an image in your HTML or CSS, reference it relative to the compiled CSS file or the HTML file, depending on your project's build setup.

Example in HTML:
