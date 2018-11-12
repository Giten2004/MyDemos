set nocompatible              " be iMproved, required
filetype off                  " required

" 启用vundle来管理vim插件
set rtp+=~/.vim/bundle/Vundle.vim
call vundle#begin()

" 安装插件写在这之后
" let Vundle manage Vundle, required
Plugin 'VundleVim/Vundle.vim'
Plugin 'Valloric/YouCompleteMe'
" 安装插件写在这之前
call vundle#end()            " required
filetype plugin on           " required

" 常用命令
" :PluginList       - 查看已经安装的插件
" :PluginInstall    - 安装插件
" :PluginUpdate     - 更新插件
" :PluginSearch     - 搜索插件，例如 :PluginSearch xml就能搜到xml相关的插件
" :PluginClean      - 删除插件，把安装插件对应行删除，然后执行这个命令即可
" h: vundle         - 获取帮助
" vundle的配置到此结束，下面是你自己的配置

" colorscheme desert

set backspace=indent,eol,start
set tabstop=4
set softtabstop=4
set shiftwidth=4
set sm
set matchtime=1

set sw=4          "自动缩进时，缩进尺寸为4个空格
set ts=4          "tab宽度为4个字符
set et            "编辑时将所有tab替换为空格
set smarttab      "删除时，一个删除键删除4个空格
set selection=inclusive     "指定在选择文本时，光标所在位置也属于被选中范围
set autoindent      "设置自动缩进

set number           "显示行号
set ru               "显示vim状态栏，显示行号、列号
set hls              "搜索时高亮显示被找到的文本

set encoding=utf-8 "设置当前字符编码为UTF-8
set fileencodings=ucs-bom,utf-8,cp936,gb18030,big5,euc-jp,euc-kr,latin1 "如果文件编码与vim编码不一致，按顺序尝试解码，如果成功则将fileencoding设置为这个值，在文件保存/打开时，自动对文件编解码转换

syntax on "打开关键字上色
filetype plugin indent on "启动文件类型自动识别、文件类型相关插件、按文件类型的自动缩进


if has("autocmd")
    autocmd FileType python setlocal et sta sw=4 sts=4
endif
