import PerfectScrollbar from 'perfect-scrollbar';

class Menu {
  constructor(el, config = {}, _PS = null) {
    this._el = el;
    this._animate = config.animate !== false;
    this._accordion = config.accordion !== false;
    this._closeChildren = Boolean(config.closeChildren);

    this._onOpen = config.onOpen || (() => {});
    this._onOpened = config.onOpened || (() => {});
    this._onClose = config.onClose || (() => {});
    this._onClosed = config.onClosed || (() => {});

    this._psScroll = null;
    this._topParent = null;
    this._menuBgClass = null;

    el.classList.add('menu');
    el.classList[this._animate ? 'remove' : 'add']('menu-no-animation'); // check

    el.classList.add('menu-vertical');

    const PerfectScrollbarLib = _PS || PerfectScrollbar;

    const menuInner = el.querySelector('.menu-inner');
    if (menuInner) {
      if (PerfectScrollbarLib) {
        this._scrollbar = new PerfectScrollbarLib(menuInner, {
          suppressScrollX: true,
          wheelPropagation: !Menu._hasClass('layout-menu-fixed layout-menu-fixed-offcanvas'),
        });

        window.Helpers.menuPsScroll = this._scrollbar;
      } else {
        console.log('PerfectScrollbar library is missing.');
        menuInner.classList.add('overflow-auto');
      }
    } else {
      console.error('No .menu-inner element found.');
    }

    // Add data attribute for bg color class of menu
    const menuClassList = el.classList;

    for (let i = 0; i < menuClassList.length; i++) {
      if (menuClassList[i].startsWith('bg-')) {
        this._menuBgClass = menuClassList[i];
      }
    }
    el.setAttribute('data-bg-class', this._menuBgClass);

    this._bindEvents();

    // Link menu instance to element
    el.menuInstance = this;
  }

  _bindEvents() {
    // Click Event
    this._evntElClick = e => {
      // Find top parent element
      if (e.target.closest('ul') && e.target.closest('ul').classList.contains('menu-inner')) {
        const menuItem = Menu._findParent(e.target, 'menu-item', false);

        if (menuItem) this._topParent = menuItem.childNodes[0];
      }

      const toggleLink = e.target.classList.contains('menu-toggle')
        ? e.target
        : Menu._findParent(e.target, 'menu-toggle', false);

      if (toggleLink) {
        e.preventDefault();

        if (toggleLink.getAttribute('data-hover') !== 'true') {
          this.toggle(toggleLink);
        }
      }
    };
    if (window.Helpers.isMobileDevice) this._el.addEventListener('click', this._evntElClick);

    this._evntWindowResize = () => {
      this.update();
      if (this._lastWidth !== window.innerWidth) {
        this._lastWidth = window.innerWidth;
        this.update();
      }

      const horizontalMenuTemplate = document.querySelector('[data-template^=\'horizontal-menu\']');
      if (!this._horizontal && !horizontalMenuTemplate) this.manageScroll();
    };
    window.addEventListener('resize', this._evntWindowResize);
  }

  manageScroll() {
    const menuInner = document.querySelector('.menu-inner');

    if (window.innerWidth < window.Helpers.LAYOUT_BREAKPOINT) {
      if (this._scrollbar !== null) {
        this._scrollbar.destroy();
        this._scrollbar = null;
      }
      menuInner.classList.add('overflow-auto');
    } else {
      if (this._scrollbar === null) {
        const menuScroll = new PerfectScrollbar(menuInner, {
          suppressScrollX: true,
          wheelPropagation: !Menu._hasClass('layout-menu-fixed layout-menu-fixed-offcanvas'),
        });
        this._scrollbar = menuScroll;
        window.Helpers.menuPsScroll = this._scrollbar;
      }
      menuInner.classList.remove('overflow-auto');
    }
  }

  static _hasClass(className) {
    return document.documentElement.classList.contains(className);
  }

  static _findParent(el, className, stopAtClass) {
    while (el) {
      if (el.classList && el.classList.contains(className)) {
        return el;
      }
      if (stopAtClass && el.classList && el.classList.contains(stopAtClass)) {
        return null;
      }
      el = el.parentElement;
    }
    return null;
  }

  // ... other methods ...
}

export { Menu };
