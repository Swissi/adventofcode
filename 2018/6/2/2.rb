def get_manhattan_distance(x1, y1, x2, y2)
  # d((1,5),(6,1) = | 1 - 6 | + | 5 - 1 | = 9
  a = (x1 - x2).abs
  b = (y1 - y2).abs
  a + b
end

lines = File.readlines('i.txt')
coords = []
min_x = 10_000
max_x = 0
min_y = 10_000
max_y = 0


lines.each_with_index do |line,i|
  arr = line.split(',')
  x = Integer(arr[0])
  y = Integer(arr[1])

  min_x = x - 1 if x < min_x

  max_x = x + 1 if x > max_x

  min_y = y - 1 if y < min_y

  max_y = y if y > max_y

  coords[i] = [x,y]
end

safe_regions = 0

(min_y..max_y).each do |y|
  (min_x..max_x).each do |x|
    sum_d = 0
    distances = {}
    coords.each_with_index do |c, i|
      d = get_manhattan_distance(x, y, c[0], c[1])
      distances[i] = d
      sum_d += d
    end

    if sum_d < 10_000
      print '#'
      safe_regions += 1
    else
      print '.'
    end
  end
  puts ''
end

puts safe_regions

puts 'end of code'







