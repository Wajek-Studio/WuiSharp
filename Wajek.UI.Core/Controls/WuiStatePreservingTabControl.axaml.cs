using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Wajek.UI.Core.Controls;

/// <summary>
/// Wrapper TabControl dasar. Gunakan seperti TabControl biasa dengan TabItem children.
/// Ke depan: state preservation akan ditambahkan di sini.
/// </summary>
public partial class WuiStatePreservingTabControl : UserControl
{
    private TabControl? _tabControl;

    // ── SelectedIndex ────────────────────────────────────
    public static readonly DirectProperty<WuiStatePreservingTabControl, int> SelectedIndexProperty =
        AvaloniaProperty.RegisterDirect<WuiStatePreservingTabControl, int>(
            nameof(SelectedIndex),
            o => o.SelectedIndex,
            (o, v) => o.SelectedIndex = v,
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (SetAndRaise(SelectedIndexProperty, ref _selectedIndex, value))
            {
                if (_tabControl is not null)
                    _tabControl.SelectedIndex = value;
            }
        }
    }

    // ── SelectedItem ─────────────────────────────────────
    public static readonly DirectProperty<WuiStatePreservingTabControl, object?> SelectedItemProperty =
        AvaloniaProperty.RegisterDirect<WuiStatePreservingTabControl, object?>(
            nameof(SelectedItem),
            o => o.SelectedItem,
            (o, v) => o.SelectedItem = v,
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    private object? _selectedItem;
    public object? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (SetAndRaise(SelectedItemProperty, ref _selectedItem, value))
            {
                if (_tabControl is not null)
                    _tabControl.SelectedItem = value;
            }
        }
    }

    // ── Items (ekspos TabItem collection dari TabControl) ─
    public ItemCollection Items => _tabControl?.Items
        ?? throw new InvalidOperationException("TabControl not initialized");

    // ── Constructor ──────────────────────────────────────
    public WuiStatePreservingTabControl()
    {
        InitializeComponent();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _tabControl = e.NameScope.Find<TabControl>("PART_TabControl");

        if (_tabControl is not null)
        {
            // Forward selection changes
            _tabControl.SelectionChanged += (s, args) =>
            {
                SelectedIndex = _tabControl.SelectedIndex;
                SelectedItem = _tabControl.SelectedItem;
            };
        }
    }
}

