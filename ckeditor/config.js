/**
 * @license Copyright (c) 2003-2020, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.contentsCss = '../Content/custom-font.css';
    //The next line will add the new font to the combobox in CKEditor
    config.font_names = 'Proxima Nova Font;' + config.font_names;
};
