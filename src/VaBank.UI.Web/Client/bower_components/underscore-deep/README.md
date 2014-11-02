# underscore-deep

##Usage

###  _.deep
```javascript
var obj = {
  a: {
    b: {
      c: {
        d: ['e', 'f', 'g']
      }
    }
  }
};
```

#### Get deep value
```javascript
_.deep(obj, 'a.b.c.d[2]'); // 'g'
```

#### Set deep value
```javascript
_.deep(obj, 'a.b.c.d[2]', 'george');

_.deep(obj, 'a.b.c.d[2]'); // 'george'
```

### _.pluckDeep

```javascript
var arr = [{
  deeply: {
    nested: 'foo'
  }
}, {
  deeply: {
    nested: 'bar'
  }
}];

_.pluckDeep(arr, 'deeply.nested'); // ['foo', 'bar']
```


### _.unpick

Return a copy of an object containing all but the blacklisted properties.


## Notes

To use underscore-deep, copy the underscore.deep.js file into your project and include it after underscore. It's also available via bower.

$ bower install underscore-deep
License: MIT

---

### Disclaimer

Author of this code is Dave Furfero (https://github.com/furf), I just needed to convert it to a bower package
Original source: https://gist.github.com/furf/3208381
