# DevLearn

## Instalation


1. Create Database DevLearn by running the script
2. Setup RabbitMQ locally, run service on default port.
3. Build solution
4. Run Admin site, add some events, and scores.
5. Run State Server app

Start services: 

```
dotnet run
```



Browser:

```html
<script src="https://unpkg.com/turndown/dist/turndown.js"></script>
```


## Usage

```js
// For Node.js
var TurndownService = require('turndown')

var turndownService = new TurndownService()
var markdown = turndownService.turndown('<h1>Hello world!</h1>')
```


```js
var markdown = turndownService.turndown(document.getElementById('content'))
```

## Options


`addRule` returns the `TurndownService` instance for chaining.

See **Extending with Rules** below.

### `filter` String|Array|Function

The filter property determines whether or not an element should be replaced with the rule's `replacement`. DOM nodes can be selected simply using a tag name or an array of tag names:

 * `filter: 'p'` will select `<p>` elements
 * `filter: ['em', 'i']` will select `<em>` or `<i>` elements


### Special Rules

**Blank rule** determines how to handle blank elements. It overrides every rule (even those added via `addRule`). A node is blank if it only contains whitespace, and it's not an `<a>`, `<td>`,`<th>` or a void element. Its behaviour can be customised using the `blankReplacement` option.


### Rule Precedence

Turndown iterates over the set of rules, and picks the first one that matches satifies the `filter`. The following list describes the order of precedence:

1. Blank rule
2. Added rules (optional)
3. Commonmark rules
4. Keep rules
5. Remove rules
6. Default rule


## License

devlearn is copyright © 2018+ Kaizen Digital and released under the MIT license.