// -------------------------------------------------------------------
// markItUp!
// -------------------------------------------------------------------
// Copyright (C) 2008 Jay Salvat
// http://markitup.jaysalvat.com/
// -------------------------------------------------------------------
// MarkDown tags example
// http://en.wikipedia.org/wiki/Markdown
// http://daringfireball.net/projects/markdown/
// -------------------------------------------------------------------
// Feel free to add more tags
// -------------------------------------------------------------------
mySettings = {
    previewParserPath: '',
    onEnter: {keepDefault:false, replaceWith:'  \n'},
//    onShiftEnter: { keepDefault: false, replaceWith:'  \n' },
    markupSet: [
        { name: 'Bold', key: 'B', openWith: '**', closeWith: '**' },
        { name: 'Italic', key: 'I', openWith: '_', closeWith: '_' },
        { separator: '---------------' },
        { name: 'Bulleted List', openWith: '- ' },
        { name: 'Numeric List', openWith: function(markItUp) {
            return markItUp.line + '. ';
        } 
        },
        { separator: '---------------' },
        { name: 'Picture', key: 'P', replaceWith: '![[![Alternative text]!]]([![Url:!:http://]!] "[![Title]!]")' },
        { name: 'Link', key: 'L', openWith: '[', closeWith: ']([![Url:!:http://]!] "[![Title]!]")', placeHolder: 'Your text to link here...' },
        
        { name: 'Quotes', openWith: '> ' },
        { name: 'Code Block / Code', openWith: '(!(\t|!|`)!)', closeWith: '(!(`)!)' },
        { separator: '---------------' },
        { name: 'Preview', call: 'preview', className: "preview" },
        { name: 'Close Preview', call: 'killPreview', className: "killPreview" }
    ]
}



// mIu nameSpace to avoid conflict.
miu = {
    markdownTitle: function(markItUp, xchar) {
        heading = '';
        n = $.trim(markItUp.selection||markItUp.placeHolder).length;
        for(i = 0; i < n; i++) {
            heading += xchar;
        }
        return '\n'+heading;
    }
}

